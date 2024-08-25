using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Conversion.Models;
using Conversion.Models.OutputSettings;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Conversion.Extensions;

public static partial class ExtensionMethods
{
	/// <summary>
	/// Extends the target object with the properties of the source object. If greedy is true, the target properties will be overwritten.
	/// </summary>
	/// <param name="target"></param>
	/// <param name="source"></param>
	/// <param name="greedy"></param>
	public static void ExtendWith<TType>(this TType target, TType source, bool greedy = false)
	{
		ArgumentNullException.ThrowIfNull(nameof(target));
		ArgumentNullException.ThrowIfNull(nameof(source));
		Type typeTarget = target!.GetType();
		Type typeSource = source!.GetType();
		if (typeTarget != typeSource)
		{
			throw new ArgumentException("The target and source must be of the same type.");
		}

		PropertyInfo[] sourceProperties = typeSource.GetProperties();
		foreach (PropertyInfo sourceProperty in sourceProperties)
		{
			if (sourceProperty != null && typeTarget != null)
			{
				var targetProperty = typeTarget?.GetProperty(sourceProperty.Name);
				if (targetProperty != null && targetProperty.CanWrite)
				{
					var value = sourceProperty.GetValue(source, null);
					var targetTypeDefault = targetProperty.PropertyType.IsValueType ? Activator.CreateInstance(targetProperty.PropertyType) : null;
					var valueIsValid = value != targetTypeDefault;
					if (valueIsValid || greedy)
					{
						targetProperty.SetValue(target, value, null);
					}
				}
			}
		}
	}

	public static ConversionOptions ToFFmpegOptions(this OutputSetting setting, ConversionOptions? extendedWith = null)
	{
		var output = new ConversionOptions
		{
			RemoveAudio = !setting.WithAudio,
			VideoCodec = VideoCodec.libx264,
			VideoSize = VideoSize.Hd720,
			VideoBitRate = 1000
		};

		if (setting.MaxBitrate != null || extendedWith?.VideoBitRate != null)
		{
			output.VideoBitRate = setting.MaxBitrate ?? extendedWith?.VideoBitRate;
		}

		if (setting.OutputPresetName != null)
		{
			output.VideoSize = OutputSizes.Get(setting.OutputPresetName.Value).FFmpegClosestMatch;
		}

		if (extendedWith != null)
		{
			// Extend the options with the provided options.
			output.ExtendWith(extendedWith);
		}

		return output;
	}
}
