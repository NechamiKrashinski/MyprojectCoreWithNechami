using project.Interfaces;

namespace project.Models
{
    public class CurrentUser : ILogin<CurrentUser>
    {
        public int Id { get; set; }
        public Role role { get; set; }

        private CurrentUser _currentUser;

        // מימוש השיטה כך שהיא מחזירה את סוג T
        public CurrentUser GetCurrentUser()
        {
            return _currentUser;
        }

        // מימוש השיטה כך שהיא מקבלת את סוג T
        public void SetCurrentUser(CurrentUser user)
        {
            _currentUser = user;
        }
    }
}
