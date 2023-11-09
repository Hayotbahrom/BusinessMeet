using BusinessMeet.Api.Controllersl;
using BusinessMeet.Api.Moddels;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.Companys;
using BusinessMeet.Service.DTOs.Users;
using BusinessMeet.Service.Interfaces.Companys;
using BusinessMeet.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace BusinessMeet.Api.Controllers.CompanyControllers;

public class CompaniesController : BaseController
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _companyService.RetrieveAllAsync(@params)
        };
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _companyService.RetrieveByIdAsync(id)
        };

        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromForm] CompanyForCreationDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _companyService.AddAsync(dto)
        };
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _companyService.RemoveAsync(id)
        };
        return Ok(response);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long Id, [FromForm] CompanyForUpdateDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _companyService.ModifyAsync(Id, dto)
        };

        return Ok(response);
    }
}
