namespace PharmacyBackend.Contracts.RepositoryContracts.DeleteInterface
{
    public interface IDeleteRepositoryContracts
    {
        public Task<bool> DeleteEmployeeByIdAsync(int id);
        public Task<bool> DeleteAccountByIdAsync(int id);
    }
}
