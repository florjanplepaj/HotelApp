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
	public class DataContoroller : ControllerBase
	{
		private readonly IDataRepository _dataRepository;
		private readonly IMapper _mapper;

		public DataContoroller(IDataRepository dataRepository, IMapper mapper) {
			_dataRepository = dataRepository;
			_mapper = mapper;
		}
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<BrowsingData>))]
		public IActionResult GetClients()
		{
			var data = _mapper.Map<List<DataDto>>(_dataRepository.GetBrowsingData());
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(data);
		}

		[HttpGet("DataId")]
		[ProducesResponseType(200, Type = typeof(BrowsingData))]
		[ProducesResponseType(400)]
		public IActionResult GetClientbyId(int dataId)
		{
			if (!_dataRepository.DataExist(dataId))
			{
				return NotFound();
			}

			var data = _mapper.Map<DataDto>(_dataRepository.GetData(dataId));
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);

			}
			return Ok(data);

		}
		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult CreateData([FromBody] DataDto datacreate)
		{
			if (datacreate == null)
			{
				return BadRequest(ModelState);
			}

			var data = _dataRepository.GetBrowsingData().Where(s =>
			s.BrowsingId == datacreate.BrowsingId).FirstOrDefault();

			if (data != null)
			{
				ModelState.AddModelError("", "Data already exists");
				return StatusCode(422, ModelState);
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var dataMap = _mapper.Map<BrowsingData>(datacreate);

			if (!_dataRepository.CreateData(dataMap))
			{
				ModelState.AddModelError("", "Somthing went wrong while saving");
				return StatusCode(500, ModelState);
			}

			return Ok("Succesfully created!");

		}
		[HttpDelete("dataId")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult Deletedata(int dataId)
		{
			if (!_dataRepository.DataExist(dataId))
			{
				return NotFound();
			}
			var dataToDelete = _dataRepository.GetData(dataId);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			if (!_dataRepository.DeleteData(dataToDelete))
			{
				ModelState.AddModelError("", "Somthing went wrong deleting data");
			}
			return NoContent();
		}
	}
}
