using Journey.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journer.Repository
{
    public interface IRepository
    {
        public void AddCustomer(RegisterDTO registerDTO);


        public void AddDest(AddAdminDestinationDTO destinationDTO);

        public List<GetAdminDestinationDTO> getAdminDestination();

        public void UpdateDest(UpdateAdminDestinationDTO updateAdminDestinationDTO);

        public void DeleteDest(int id);


        public (bool IsValid, bool IsAdmin) ValidateUser(string UserName, string Password);

        public void AddBook(AddBookUserDTO addBookUserDTO);

    }
}
