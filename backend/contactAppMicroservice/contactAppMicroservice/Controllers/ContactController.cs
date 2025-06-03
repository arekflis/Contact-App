using contactAppMicroservice.DTO.Request;
using contactAppMicroservice.DTO.Response;
using contactAppMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace contactAppMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController(IContactService contactService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<ContactResponse>>> getAllContacts()
        {
            var contactsResponse = await contactService.getAllContacts();

            if (contactsResponse is null || contactsResponse.Count == 0)
                return NotFound();

            return Ok(contactsResponse);
        }

        [HttpGet("{ContactId}")]
        public async Task<ActionResult<ContactDetailsResponse>> getContactById(Guid ContactId)
        {
            var contactDetailsResponse = await contactService.getContactById(ContactId);

            if (contactDetailsResponse is null)
                return NotFound();

            return Ok(contactDetailsResponse);
        }

        [Authorize]
        [HttpDelete("{ContactId}")]
        public async Task<ActionResult<string>> deleteContact(Guid ContactId, DeleteContactRequest deleteContactRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await contactService.deleteContact(ContactId, deleteContactRequest.Password);

            if (!result) return BadRequest("Invalid password or contact id");

            return Ok("Contact successfully deleted");
        }
    }
}
