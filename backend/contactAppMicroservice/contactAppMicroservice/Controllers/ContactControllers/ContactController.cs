using AutoMapper;
using contactAppMicroservice.DTO.Request;
using contactAppMicroservice.DTO.Response;
using contactAppMicroservice.Services.ContactServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace contactAppMicroservice.Controllers.Contact
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController(IContactService contactService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<ContactResponse>>> getAllContacts()
        {
            var contactsResponse = await contactService.getAllContactsAsync();

            if (contactsResponse is null || contactsResponse.Count == 0)
                return NotFound();

            return Ok(contactsResponse);
        }

        [HttpGet("{ContactId}")]
        public async Task<ActionResult<ContactDetailsResponse>> getContactById(Guid ContactId)
        {
            var contactDetailsResponse = await contactService.getContactByIdAsync(ContactId);

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

            var result = await contactService.deleteContactAsync(ContactId, deleteContactRequest.Password);

            if (!result) return BadRequest("Invalid password or contact id");

            return Ok("Contact successfully deleted");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ContactDetailsResponse>> addNewContact(ContactRequest contactRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await contactService.addNewContactAsync(contactRequest);
                return CreatedAtAction(nameof(getContactById), new { result.ContactId }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{contactId}")]
        public async Task<IActionResult> updateContact(Guid contactId, ContactRequest contactRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await contactService.updateContactAsync(contactId, contactRequest);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
