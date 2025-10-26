using PharmacyBackend.DataContext;

namespace PharmacyBackend.Helper
{
    public class AccountHelper
    {
        private readonly PharmacyContext _context;
        public AccountHelper(PharmacyContext context)
        {
            _context = context;
        }

        public bool AccountUsernameMultiplicationValidation(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;
            var validation = _context.Accounts.Any(a => a.Username.Equals(userName));
            if (validation)
                return false;
            return true;
        }
    }
}
