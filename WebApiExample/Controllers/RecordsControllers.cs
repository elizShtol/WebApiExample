using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiExample.Models;

namespace WebApiExample.Controllers;

[Authorize]
[Route("records")]
public class RecordsControllers: ControllerBase
{
    private static Dictionary<Guid, Record> records = new();

    [HttpPost(Name = "AddRecord")]
    [ProducesResponseType(typeof(Record), 201)]
    public IActionResult AddRecord([FromBody]AddRecordRequest addRecordRequest)
    {
        var record = new Record { Id = Guid.NewGuid(), Text = addRecordRequest.Text };
        records.Add(record.Id, record);
        return Ok(record);
    }
    
    [HttpGet("{id:guid}", Name = "GetRecord")]
    [ProducesResponseType(typeof(Record), 200)]
    public IActionResult GetRecord([FromRoute] Guid id)
    {
        return records.TryGetValue(id, out var record) ? Ok(record) : BadRequest();
    }
    
    [HttpPatch("{id:guid}", Name = "UpdateRecord")]
    [ProducesResponseType(typeof(Record), 200)]
    public IActionResult UpdateRecord([FromRoute] Guid id, [FromBody] UpdateRecordRequest updateRecordRequest)
    {
        if (records.ContainsKey(id))
        {
            var record = records[id];
            record.Text = updateRecordRequest.NewText;
            return Ok(record);
        }
        return BadRequest();
    }
    
    [HttpGet(Name = "GetRecords")]
    [ProducesResponseType(typeof(RecordsPage), 200)]
    public IActionResult GetRecords()
    {
        return Ok(new RecordsPage(){Records = records.Values.ToArray(), TotalCount = records.Count});
    }
    
    [HttpDelete("{id:guid}", Name = "GetRecord")]
    public IActionResult DeleteRecord([FromRoute] Guid id)
    {
        records.Remove(id);
        return Ok();
    }
}