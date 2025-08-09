namespace PharmacyBackend.DTOs.CategoryDTOs
{
    public class UpdateCategoryDto : CreateCategoryDto
    {
        public bool IsActive { get; set; }
    }
}
