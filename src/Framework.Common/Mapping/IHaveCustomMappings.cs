using AutoMapper;

namespace Framework.Common.Mapping
{
	public interface IHaveCustomMappings
	{
		void CreateMappings(IMapperConfigurationExpression configuration);
	}
}