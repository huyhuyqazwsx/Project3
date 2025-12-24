using Microsoft.AspNetCore.Mvc;
using Project3.Application.Dtos.Class;
using Project3.Application.Dtos.Subject;
using Project3.Application.Interfaces;
using Project3.Domain.Entities;
using System.Net;

namespace Project3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _classService.GetAllAsync();
                var dto = result.Select(x => new ResponeClass
                {
                    Id = x.Id,
                    Name = x.Name,
                    SubjectId = x.SubjectId,
                    TeacherId = x.TeacherId
                });
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _classService.GetByIdAsync(id);
                if (result == null) return NotFound();
                else
                {
                    var dto = new ResponeClass
                    {
                        Id = result.Id,
                        Name = result.Name,
                        SubjectId= result.SubjectId,
                        TeacherId = result.TeacherId
                    };
                    return Ok(dto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ResquestCreateClass dto)
        {
            try
            {
                if (dto == null) return BadRequest("Thiếu dữ liệu");
                var result = new Class
                {
                    Name = dto.Name,
                    SubjectId = dto.SubjectId,
                    TeacherId = dto.TeacherId
                };
                await _classService.CreateAsync(result);

                return Ok(new
                {
                    message = "Created successfully",
                    id = result.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _classService.DeleteAsync(id);
                if (result == false) return NotFound();

                else return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-students")]
        public async Task<IActionResult> AddStudents([FromBody] UpdateStudentsInClassDto dto)
        {
            if (dto == null || dto.StudentIds == null || dto.StudentIds.Count == 0)
                return BadRequest("Danh sách sinh viên rỗng");

            try
            {
                var result = await _classService
                    .AddStudentsAsync(dto.ClassId, dto.StudentIds);

                return Ok(new
                {
                    message = "Thêm sinh viên thành công",
                    added = result.Added,
                    skipped = result.AlreadyExists
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("remove-students")]
        public async Task<IActionResult> RemoveStudents([FromBody] UpdateStudentsInClassDto dto)
        {
            if (dto == null || dto.StudentIds == null || dto.StudentIds.Count == 0)
                return BadRequest("Danh sách sinh viên rỗng");

            try
            {
                var result = await _classService
                    .RemoveStudentsAsync(dto.ClassId, dto.StudentIds);

                return Ok(new
                {
                    message = "Xoá sinh viên thành công",
                    removed = result.Removed
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{classId}/students")]
        public async Task<IActionResult> GetStudentsInClass(int classId)
        {
            try
            {
                var students = await _classService
                    .GetStudentsInClassAsync(classId);

                return Ok(new
                {
                    classId,
                    total = students.Count,
                    students
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
