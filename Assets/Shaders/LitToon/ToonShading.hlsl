void ToonShading_float(float3 Normal, float ToonRampSmoothness, float3 ClipSpacePos, float3 WorldPos, float4 ToonRampTint, float ToonRampOffset,
	out float3 ToonRampOutput, out float3 LightDirection) 
{
#ifdef SHADERGRAPH_PREVIEW
	ToonRampOutput = float3(0.5, 0.5, 0);
	LightDirection = float3(0.5, 0.5, 0);
#else
	#if SHADOWS_SCREEN
		half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
	#else
		half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	#endif
	
	#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
		Light light = GetMainLight(shadowCoord);
	#else
		Light light = GetMainLight();
	#endif
	
	half d = dot(Normal, light.direction) * 0.5 + 0.5;
	half toonRamp = smoothstep(ToonRampOffset, ToonRampOffset + ToonRampSmoothness, d);
	toonRamp *= light.shadowAttenuation;
	
	ToonRampOutput = light.color * (toonRamp + ToonRampTint);
	LightDirection = light.direction;
#endif
}