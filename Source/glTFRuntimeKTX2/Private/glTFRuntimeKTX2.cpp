// Copyright Roberto De Ioris

#include "glTFRuntimeKTX2.h"
#include "glTFRuntimeParser.h"
THIRD_PARTY_INCLUDES_START
#include <ktx.h>
THIRD_PARTY_INCLUDES_END

#define LOCTEXT_NAMESPACE "FglTFRuntimeKTX2Module"

void FglTFRuntimeKTX2Module::StartupModule()
{
	// extract the right ImageIndex
	FglTFRuntimeParser::OnTextureImageIndex.AddLambda([](TSharedRef<FglTFRuntimeParser> Parser, TSharedRef<FJsonObject> JsonTextureObject, int64& ImageIndex)
		{
			// check for used extensions
			if (!Parser->ExtensionsUsed.Contains("KHR_texture_basisu"))
			{
				return;
			}

	ImageIndex = Parser->GetJsonExtensionObjectIndex(JsonTextureObject, "KHR_texture_basisu", "source", INDEX_NONE);
		});

	// extract ImagePixels
	FglTFRuntimeParser::OnTexturePixels.AddLambda([](TSharedRef<FglTFRuntimeParser> Parser, TSharedRef<FJsonObject> JsonImageObject, TArray64<uint8>& CompressedPixels, int32& Width, int32& Height, TArray64<uint8>& UncompressedPixels)
		{
			// skip if already processed
			if (UncompressedPixels.Num() > 0)
			{
				return;
			}

	// check for used extensions
	if (!Parser->ExtensionsUsed.Contains("KHR_texture_basisu"))
	{
		return;
	}

	const FString MimeType = Parser->GetJsonObjectString(JsonImageObject, "mimeType", "");
	// skip non ktx2 mimeType (can be empty)
	if (MimeType != "" && MimeType != "image/ktx2")
	{
		return;
	}

	ktxTexture2* KTX2Texture;
	KTX_error_code KTXResult;
	ktx_size_t KTX2Offset;

	KTXResult = ktxTexture2_CreateFromMemory(CompressedPixels.GetData(), CompressedPixels.Num(), KTX_TEXTURE_CREATE_LOAD_IMAGE_DATA_BIT, &KTX2Texture);
	if (KTXResult != KTX_SUCCESS)
	{
		if (MimeType != "" || (MimeType == "" && KTXResult != KTX_UNKNOWN_FILE_FORMAT))
		{
			Parser->AddError("FglTFRuntimeKTX2Module::StartupModule()", "Unable to load KTX2 texture");
		}
		return;
	}

	if (KTX2Texture->numLevels > 0)
	{
		if (ktxTexture2_NeedsTranscoding(KTX2Texture))
		{
			KTXResult = ktxTexture2_TranscodeBasis(KTX2Texture, KTX_TTF_RGBA32, 0);
			if (KTXResult != KTX_SUCCESS)
			{
				ktxTexture_Destroy(ktxTexture(KTX2Texture));
				Parser->AddError("FglTFRuntimeKTX2Module::StartupModule()", "Unable to transcode KTX2 texture");
				return;
			}
		}
		KTXResult = ktxTexture_GetImageOffset(ktxTexture(KTX2Texture), 0, 0, 0, &KTX2Offset);
		if (KTXResult != KTX_SUCCESS)
		{
			ktxTexture_Destroy(ktxTexture(KTX2Texture));
			Parser->AddError("FglTFRuntimeKTX2Module::StartupModule()", "Unable to get KTX2 texture offset");
			return;
		}

		Width = KTX2Texture->baseWidth;
		Height = KTX2Texture->baseHeight;

		ktx_uint8_t* KTX2ImageData = ktxTexture_GetData(ktxTexture(KTX2Texture)) + KTX2Offset;

		const int64 ImageSize = Width * Height * 4;
		for (int64 IndexR = 0; IndexR < ImageSize; IndexR += 4)
		{
			Swap(KTX2ImageData[IndexR], KTX2ImageData[IndexR + 2]);
		}
		UncompressedPixels.Append(KTX2ImageData, ImageSize);
	}

	ktxTexture_Destroy(ktxTexture(KTX2Texture));
		});
}

void FglTFRuntimeKTX2Module::ShutdownModule()
{

}

#undef LOCTEXT_NAMESPACE

IMPLEMENT_MODULE(FglTFRuntimeKTX2Module, glTFRuntimeKTX2)