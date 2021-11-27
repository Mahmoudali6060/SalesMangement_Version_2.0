using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Users
{
    public class UserOperationsRepo : IUserOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<User> userEntity;

        public UserOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            userEntity = context.Set<User>();
        }

        public IEnumerable<User> GetAll()
        {
            return userEntity.Include("Role").AsEnumerable();
        }

        public UserDTO GetUserDTOById(long id)
        {
            User user = userEntity.Include("Role").SingleOrDefault(s => s.Id == id);
            UserDTO userDTO = Map(user);
            userDTO.ImageBase64 = user.ImageUrl != null ? GetImageBase64String(user.ImageUrl) : null;
            return userDTO;
        }

        public User GetById(long id)
        {
            return userEntity.Include("Role").SingleOrDefault(s => s.Id == id);
        }

        public bool Add(UserDTO userDTO)
        {
            User user = Map(userDTO);
            context.Entry(user).State = EntityState.Added;
            var result = context.SaveChanges();
            if (result > 0)
            {
                SaveProfileImage(user.ImageUrl, userDTO.ImageBase64);
                return true;
            }
            return false;
        }

        public bool Update(UserDTO userDTO)
        {
            User user = Map(userDTO);
            context.Users.Update(user);
            var result = context.SaveChanges();
            if (result > 0)
            {
                DeleteImage(userDTO.ImageUrl);
                DeleteImage(user.ImageUrl);
                SaveProfileImage(user.ImageUrl, userDTO.ImageBase64);
                return true;
            }
            return false;
        }


        public bool Delete(long id)
        {
            User user = GetById(id);
            userEntity.Remove(user);
            context.SaveChanges();
            DeleteImage(user.ImageUrl);
            return true;
        }

        public User Login(string username, string password)
        {
            return context.Users.Include("Role").FirstOrDefault(x => x.Username == username && x.Password == username);
        }

        private void SaveProfileImage(string imgName, string base64imageString)
        {
            if (base64imageString != null)
            {
                var splittedText = base64imageString.Split(",");
                var img = splittedText[splittedText.Length - 1];

                string filePath = "wwwroot\\images\\Users\\" + imgName;
                File.WriteAllBytes(filePath, Convert.FromBase64String(img));
            }
        }

        private bool DeleteImage(string imageUrl)
        {
            try
            {
                string filePath = "wwwroot\\images\\Users\\" + imageUrl;
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private User Map(UserDTO userDTO)
        {
            var company = context.Companys.SingleOrDefault();
            return new User()
            {
                Id = userDTO.Id,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Username = userDTO.Username,
                Password = userDTO.Password,
                RoleId = userDTO.RoleId,
                CompanyId = company.Id,
                ImageUrl = userDTO.ImageBase64 != null ? userDTO.Username + ".jpg" : null //+ "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") : null
            };
        }

        private UserDTO Map(User user)
        {
            var company = context.Companys.SingleOrDefault();
            return new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Password = user.Password,
                RoleId = user.RoleId,
                CompanyId = company.Id,
                ImageUrl = user.ImageUrl
            };
        }

        private string GetImageBase64String(string filename)
        {
            try
            {
                byte[] imageArray = System.IO.File.ReadAllBytes("wwwroot\\images\\Users\\" + filename);
                if (imageArray != null)
                {
                    return Convert.ToBase64String(imageArray);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Logout()
        {
            throw new NotImplementedException();
        }
    }
}
