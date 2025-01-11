using System;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
	private readonly Cloudinary cloudinary;
	public PhotoService(IOptions<CloudinarySettingsHelper> config)
	{
		var acct = new Account(config.Value.CloudName, config.Value.APIKey, config.Value.APISecret);
		cloudinary = new Cloudinary(acct);
	}

	public async Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile)
	{
		var uploadResult = new ImageUploadResult();

		if (formFile.Length > 0)
		{
			using var fileStream = formFile.OpenReadStream();
			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(formFile.FileName, fileStream),
				Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
				Folder = "dating-app-net8"
			};

			uploadResult = await cloudinary.UploadAsync(uploadParams);
		}
		return uploadResult;
	}

	public async Task<DeletionResult> DeletePhotoAsync(string publicID)
	{
		var deleteParams = new DeletionParams(publicID);

		return await cloudinary.DestroyAsync(deleteParams);
	}
}
