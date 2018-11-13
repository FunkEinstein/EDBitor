using DocumentParsers.Exceptions;

namespace DocumentParsers.Schemas.Elements.Builders.BuildingRules
{
    /// <summary>
    /// Encapsulate building restrictions.
    /// Should throw exception is operation is unavailable.
    /// </summary>
    public interface IBuildingRules
    {
        /// <exception cref="SchemaElementBuildingException">Throw when operation is unavailable</exception>
        void IsAvailableBuildHeader(SchemaElementBuilder builder);

        /// <exception cref="SchemaElementBuildingException">Throw when operation is unavailable</exception>
        void IsAvailableBuildElement(SchemaElementBuilder builder);

        /// <exception cref="SchemaElementBuildingException">Throw when operation is unavailable</exception>
        void IsAvailableBuildArray(SchemaElementBuilder builder);

        /// <exception cref="SchemaElementBuildingException">Throw when operation is unavailable</exception>
        void IsAvailableBuildComment(SchemaElementBuilder builder);

        /// <exception cref="SchemaElementBuildingException">Throw when operation is unavailable</exception>
        void IsAvailableBuildConstant(SchemaElementBuilder builder);
    }
}
