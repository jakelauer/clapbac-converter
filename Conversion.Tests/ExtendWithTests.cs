using System;
using NUnit.Framework;
using Conversion.Extensions;

namespace Conversion.Tests;

public class Source
{
	public int IntProperty { get; set; }
	public string StringProperty { get; set; }
	public bool BoolProperty { get; set; }
}

[TestFixture]
public class ExtendWithTests
{
	[Test]
	public void ExtendWith_NonNullProperties_ExtendsTarget()
	{
		var source = new Source { IntProperty = 10, StringProperty = "Test", BoolProperty = true };
		var target = new Source();

		target.ExtendWith(source);

		Assert.Multiple(() =>
		{
			Assert.That(target.IntProperty, Is.EqualTo(10));
			Assert.That(target.StringProperty, Is.EqualTo("Test"));
			Assert.That(target.BoolProperty, Is.EqualTo(true));
		});

	}

	[Test]
	public void ExtendWith_NullProperties_DoesNotOverrideTarget()
	{
		var source = new Source { IntProperty = 10, StringProperty = null, BoolProperty = true };
		var target = new Source { StringProperty = "Initial" };

		target.ExtendWith(source);

		Assert.Multiple(() =>
		{
			Assert.That(target.IntProperty, Is.EqualTo(10));
			Assert.That(target.StringProperty, Is.EqualTo("Initial"));
			Assert.That(target.BoolProperty, Is.EqualTo(true));
		});
	}

	[Test]
	public void ExtendWith_GreedyNullProperties_DoesOverrideTarget()
	{
		var source = new Source { IntProperty = 10, StringProperty = null, BoolProperty = true };
		var target = new Source { StringProperty = "Initial" };

		target.ExtendWith(source, true);

		Assert.Multiple(() =>
		{
			Assert.That(target.IntProperty, Is.EqualTo(10));
			Assert.That(target.StringProperty, Is.EqualTo(null));
			Assert.That(target.BoolProperty, Is.EqualTo(true));
		});
	}

	[Test]
	public void ExtendWith_GreedyTrue_OverridesTarget()
	{
		var source = new Source { IntProperty = 10, StringProperty = "Test", BoolProperty = true };
		var target = new Source { IntProperty = 5, StringProperty = "Initial", BoolProperty = false };

		target.ExtendWith(source);

		Assert.Multiple(() =>
		{
			Assert.That(target.IntProperty, Is.EqualTo(10));
			Assert.That(target.StringProperty, Is.EqualTo("Test"));
			Assert.That(target.BoolProperty, Is.EqualTo(true));
		});

	}

	[Test]
	public void ExtendWith_GreedyFalse_DoesNotOverrideNonNullTarget()
	{
		var source = new Source { IntProperty = 10, StringProperty = "Test", BoolProperty = true };
		var target = new Source { IntProperty = 5, StringProperty = "Initial", BoolProperty = false };

		target.ExtendWith(source);

		Assert.Multiple(() =>
		{
			Assert.That(target.IntProperty, Is.EqualTo(10));
			Assert.That(target.StringProperty, Is.EqualTo("Test"));
			Assert.That(target.BoolProperty, Is.EqualTo(true));
		});

	}

	[Test]
	public void ExtendWith_DifferentPropertyTypes_ExtendsCorrectly()
	{
		var source = new Source { IntProperty = 10, StringProperty = "Test", BoolProperty = true };
		var target = new Source();

		target.ExtendWith(source);

		Assert.Multiple(() =>
		{
			Assert.That(target.IntProperty, Is.EqualTo(10));
			Assert.That(target.StringProperty, Is.EqualTo("Test"));
			Assert.That(target.BoolProperty, Is.EqualTo(true));
		});
	}

	[Test]
	public void ExtendWith_MissingInSource_ExtendsCorrectly()
	{
		var source = new Source { IntProperty = 10, BoolProperty = true };
		var target = new Source { StringProperty = "Test" };

		target.ExtendWith(source);

		Assert.Multiple(() =>
		{
			Assert.That(target.IntProperty, Is.EqualTo(10));
			Assert.That(target.StringProperty, Is.EqualTo("Test"));
			Assert.That(target.BoolProperty, Is.EqualTo(true));
		});
	}
}
