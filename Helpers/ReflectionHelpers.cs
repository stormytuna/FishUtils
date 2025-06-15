using System.Reflection;

namespace FishUtils.Helpers;

public static class ReflectionHelpers
{
	public static BindingFlags AllFlags {
		get => BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
	}
}
