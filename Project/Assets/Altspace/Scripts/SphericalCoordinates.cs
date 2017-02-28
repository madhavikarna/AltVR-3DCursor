using System;
using UnityEngine;

public class SphericalCoordinates
{
	public bool loopPolar = true;

	public bool loopElevation;

	private float _radius;

	private float _polar;

	private float _elevation;

	private float _minRadius;

	private float _maxRadius;

	private float _minPolar;

	private float _maxPolar;

	private float _minElevation;

	private float _maxElevation;

	public float radius
	{
		get
		{
			return _radius;
		}
		private set
		{
			 _radius = Mathf.Clamp(value,  _minRadius,  _maxRadius);
			 _radius = ((!float.IsNaN( _radius)) ?  _radius :  _minRadius);
		}
	}

	public float polar
	{
		get
		{
			return  _polar;
		}
		private set
		{
			 _polar = ((! loopPolar) ? Mathf.Clamp(value,  _minPolar,  _maxPolar) : Mathf.Repeat(value,  _maxPolar -  _minPolar));
			 _polar = ((!float.IsNaN( _polar)) ?  _polar :  _minPolar);
		}
	}

	public float elevation
	{
		get
		{
			return  _elevation;
		}
		private set
		{
			 _elevation = ((! loopElevation) ? Mathf.Clamp(value,  _minElevation,  _maxElevation) : Mathf.Repeat(value,  _maxElevation -  _minElevation));
			 _elevation = ((!float.IsNaN( _elevation)) ?  _elevation :  _minElevation);
		}
	}

	public Vector3 toCartesian
	{
		get
		{
			float num =  radius * Mathf.Cos( elevation);
			return new Vector3(num * Mathf.Cos( polar),  radius * Mathf.Sin( elevation), num * Mathf.Sin( polar));
		}
	}

	public SphericalCoordinates()
	{
	}

	public SphericalCoordinates(float radius, float polar, float elevation, float minRadius = 1f, float maxRadius = 20f, float minPolar = 0f, float maxPolar = 6.28318548f, float minElevation = 0f, float maxElevation = 1.04719758f)
	{
		 _minRadius = minRadius;
		 _maxRadius = maxRadius;
		 _minPolar = minPolar;
		 _maxPolar = maxPolar;
		 _minElevation = minElevation;
		 _maxElevation = maxElevation;
		 SetRadius(radius);
		 SetRotation(polar, elevation);
	}

	public SphericalCoordinates(Transform T, float minRadius = 1f, float maxRadius = 20f, float minPolar = 0f, float maxPolar = 6.28318548f, float minElevation = 0f, float maxElevation = 1.04719758f) : this(T.position, minRadius, maxRadius, minPolar, maxPolar, minElevation, maxElevation)
	{
	}

	public SphericalCoordinates(Vector3 cartesianCoordinate, float minRadius = 1f, float maxRadius = 20f, float minPolar = 0f, float maxPolar = 6.28318548f, float minElevation = 0f, float maxElevation = 1.04719758f)
	{
		 _minRadius = minRadius;
		 _maxRadius = maxRadius;
		 _minPolar = minPolar;
		 _maxPolar = maxPolar;
		 _minElevation = minElevation;
		 _maxElevation = maxElevation;
		 FromCartesian(cartesianCoordinate);
	}

	public SphericalCoordinates FromCartesian(Vector3 cartesianCoordinate)
	{
		if (cartesianCoordinate.x == 0f)
		{
			cartesianCoordinate.x = 1.401298E-45f;
		}
		 radius = cartesianCoordinate.magnitude;
		 polar = Mathf.Atan(cartesianCoordinate.z / cartesianCoordinate.x);
		if (cartesianCoordinate.x < 0f)
		{
			 polar += 3.14159274f;
		}
		 elevation = Mathf.Asin(cartesianCoordinate.y /  radius);
		return this;
	}

	public SphericalCoordinates RotatePolarAngle(float x)
	{
		return  Rotate(x, 0f);
	}

	public SphericalCoordinates RotateElevationAngle(float x)
	{
		return  Rotate(0f, x);
	}

	public SphericalCoordinates Rotate(float newPolar, float newElevation)
	{
		return  SetRotation( polar + newPolar,  elevation + newElevation);
	}

	public SphericalCoordinates SetPolarAngle(float x)
	{
		return  SetRotation(x,  elevation);
	}

	public SphericalCoordinates SetElevationAngle(float x)
	{
		return  SetRotation( polar, x);
	}

	public SphericalCoordinates SetRotation(float newPolar, float newElevation)
	{
		 polar = newPolar;
		 elevation = newElevation;
		return this;
	}

	public SphericalCoordinates TranslateRadius(float x)
	{
		return  SetRadius( radius + x);
	}

	public SphericalCoordinates SetRadius(float rad)
	{
		 radius = rad;
		return this;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
			{
				"[Radius] ",
				 radius,
				". [Polar] ",
				 polar,
				". [Elevation] ",
				 elevation,
				"."
			});
	}
}
