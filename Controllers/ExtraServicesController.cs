using AutoMapper;
using HotelApp1.DTO;
using HotelApp1.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using HotelApp1.Entities.Models;
using HotelApp1.Domain.Interface;

namespace HotelApp1.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ExtraServicesController : Controller
	{
		private readonly IExtraServicesRepository _extraServicesRepository;
		private readonly IMapper _mapper;

		public ExtraServicesController(IExtraServicesRepository extraServicesRepository, IMapper mapper) {
			_extraServicesRepository = extraServicesRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ExtraService>))]
		public IActionResult GetExtraServices()
		{
			var services = _mapper.Map<List<ServicesDto>>(_extraServicesRepository.GetExtraServices());
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(services);
		}

		[HttpGet("ServicesId")]
		[ProducesResponseType(200, Type = typeof(ExtraService))]
		[ProducesResponseType(400)]
		public IActionResult GetClientbyId(int servicesId)
		{
			if (!_extraServicesRepository.servicesExist(servicesId))
			{
				return NotFound();
			}

			var Client = _mapper.Map<ServicesDto>(_extraServicesRepository.GetExtraService(servicesId));
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);

			}
			return Ok(Client);

		}

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult CreateExtraService([FromBody] ServicesDto ExtraServicecreate)
		{
			if (ExtraServicecreate == null)
			{
				return BadRequest(ModelState);
			}

			var reservationService = _extraServicesRepository.GetExtraServices().Where(s =>
			s.ServicesId == ExtraServicecreate.ServicesId).FirstOrDefault();
			if (reservationService != null)
			{
				ModelState.AddModelError("", "ExtraService already exists");
				return StatusCode(422, ModelState);
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var extraServiceMap = _mapper.Map<ExtraService>(ExtraServicecreate);

			if (!_extraServicesRepository.CreateExtraService(extraServiceMap))
			{
				ModelState.AddModelError("", "Somthing went wrong while saving");
				return StatusCode(500, ModelState);
			}

			return Ok("Succesfully created!");

		}
		[HttpPut]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateRoom(int extraseviceId,
			[FromBody] ServicesDto updateservices)
		{
			if (updateservices == null)
			{
				return BadRequest(ModelState);
			}
			if (extraseviceId != updateservices.ServicesId)
			{
				return BadRequest(ModelState);
			}
			if (!_extraServicesRepository.servicesExist(extraseviceId))
			{
				return NotFound();
			}
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var extraServiceMap = _mapper.Map<ExtraService>(updateservices);

			if (!_extraServicesRepository.UpdateExtraService(extraServiceMap))
			{
				ModelState.AddModelError("", "Somthing went wrong updating services");
				return StatusCode(500, ModelState);
			}
			return Ok("Succesfully updated services");
		}
		[HttpDelete("extraserviceId")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult Deleteextraservice(int extraserviceId)
		{
			if (!_extraServicesRepository.servicesExist(extraserviceId))
			{
				return NotFound();
			}
			var extraserviceToDelete = _extraServicesRepository.GetExtraService(extraserviceId);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			if (!_extraServicesRepository.DeleteExtraService(extraserviceToDelete))
			{
				ModelState.AddModelError("", "Somthing went wrong deleting extraservice");
			}
			return NoContent();
		}
	}
}
