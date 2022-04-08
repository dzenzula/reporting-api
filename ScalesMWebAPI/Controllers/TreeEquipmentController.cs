using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScalesMWebAPI.Dtos;
using ScalesMWebAPI.Models;

namespace ScalesMWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TreeEquipmentController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;

        public TreeEquipmentController(KRRPAMONSCALESContext context)
        {
            _context = context;
        }

        //// GET: api/GetTreeDtoes
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TreeEquipmentDto>>> GetTree()
        //{
        //    return await Task.Run(() => { 
        //    List<TreeEquipmentDto> treeEquipment = new List<TreeEquipmentDto>();
        //    int index_tree = 0;
        //    var select_lp = _context.LocationPoints.ToList();
        //    foreach (var lp in select_lp)
        //    {
        //        TreeEquipmentDto root_leaf = new TreeEquipmentDto
        //        {
        //            Id = index_tree++,
        //            Id_Db = lp.Id,
        //            Text = lp.NameLocation,
        //            Type = 1
        //        };
        //        List<TreeElementDto> list_wp = new List<TreeElementDto>();
        //        var select_wp = _context.WeightPoints.Where(w => w.LocationPointId == lp.Id).ToList();
        //        foreach (var wp in select_wp)
        //        {
        //            TreeElementDto wp_el = new TreeElementDto();
        //            wp_el.Id = index_tree++;
        //            wp_el.Id_Db = wp.Id;
        //            wp_el.ParentId = lp.Id;
        //            wp_el.Text = wp.NumberScale;
        //            wp_el.Type = 2;
                        
        //                var select_wc = _context.WeightPlcs.Where(w => w.ScalesNumberId == wp.Id).ToList();
        //                List<TreeElementDto> list_plc = new List<TreeElementDto>();
        //                foreach (var wc in select_wc)
        //                {
        //                    TreeElementDto plc = new TreeElementDto();
        //                    plc.Id = index_tree++;
        //                    plc.Id_Db = wc.Id;
        //                    plc.ParentId = wc.ScalesNumberId;
        //                    plc.Text = String.Format($"{wc.NamePlc} / Tag-{wc.ServiceTag}");
        //                    plc.Type = 3;
                            
        //                    var select_wplatform = _context.WeightPlatforms.Where(w => w.WeightPlcId == plc.Id).ToList();
        //                        List<TreeElementDto> list_platform = new List<TreeElementDto>();
        //                        foreach (var platform in select_wplatform)
        //                        {
        //                            TreeElementDto plat = new TreeElementDto();
        //                            plat.Id = index_tree++;
        //                            plat.Id_Db = platform.Id;
        //                            plat.ParentId = platform.WeightPlcId;
        //                            plat.Text = platform.ScaleNumberPlatform.ToString();
        //                            plat.Type = 4;
        //                            plat.Opened = false;
        //                            plat.Children = new List<TreeElementDto>();
        //                        list_platform.Add(plat);
        //                        }
        //                        plc.Opened = list_platform.Count > 0;
        //                    plc.Children = list_platform;
        //                    list_plc.Add(plc);
        //                }
        //                wp_el.Opened = list_plc.Count > 0;
        //                wp_el.Children = list_plc;
        //            list_wp.Add(wp_el);
        //        }
        //        root_leaf.Opened = list_wp.Count > 0;
        //        root_leaf.Children = list_wp;
        //            treeEquipment.Add(root_leaf);
        //    }
        //    return treeEquipment;
        //    });
        //}

        // GET: api/GetTreeDtoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreeEquipmentDto>>> GetTreeRoot()
        {
            return await Task.Run(() => {
                List<TreeEquipmentDto> treeEquipment = new List<TreeEquipmentDto>();
                int index_tree = 0;
                var select_lp = _context.LocationPoints.ToList();
                TreeEquipmentDto root = new TreeEquipmentDto();
                root.Id = index_tree++;
                root.Text = "AMKR";
                List<TreeElementDto> list_location = new List<TreeElementDto>();
                foreach (var lp in select_lp)
                {
                    TreeElementDto node = new TreeElementDto
                    {
                        Id = index_tree++,
                        Id_Db = lp.Id,
                        Text = lp.NameLocation,
                        Type = 1
                    };
                    List<TreeElementDto> list_wp = new List<TreeElementDto>();
                    var select_wp = _context.WeightPoints.Where(w => w.LocationPointId == lp.Id).ToList();
                    foreach (var wp in select_wp)
                    {
                        TreeElementDto wp_el = new TreeElementDto();
                        wp_el.Id = index_tree++;
                        wp_el.Id_Db = wp.Id;
                        wp_el.ParentId = lp.Id;
                        wp_el.Text = wp.NumberScale;
                        wp_el.Type = 2;

                        var select_wc = _context.WeightPlcs.Where(w => w.ScalesNumberId == wp.Id).ToList();
                        List<TreeElementDto> list_plc = new List<TreeElementDto>();
                        foreach (var wc in select_wc)
                        {
                            TreeElementDto plc = new TreeElementDto();
                            plc.Id = index_tree++;
                            plc.Id_Db = wc.Id;
                            plc.ParentId = wc.ScalesNumberId;
                            plc.Text = String.Format($"{wc.NamePlc} / Tag-{wc.ServiceTag}");
                            plc.Type = 3;

                            var select_wplatform = _context.WeightPlatforms.Where(w => w.WeightPlcId == plc.Id_Db).ToList();
                            List<TreeElementDto> list_platform = new List<TreeElementDto>();
                            foreach (var platform in select_wplatform)
                            {
                                TreeElementDto plat = new TreeElementDto();
                                plat.Id = index_tree++;
                                plat.Id_Db = platform.Id;
                                plat.ParentId = platform.WeightPlcId;
                                plat.Text = platform.ScaleNumberPlatform.ToString();
                                plat.Type = 4;
                                plat.Opened = false;
                                plat.Children = new List<TreeElementDto>();
                                list_platform.Add(plat);
                            }
                            plc.Opened = false;
                            //plc.Opened = list_platform.Count > 0;
                            plc.Children = list_platform;
                            list_plc.Add(plc);
                        }
                        wp_el.Opened = false;
                        //wp_el.Opened = list_plc.Count > 0;
                        wp_el.Children = list_plc;
                        list_wp.Add(wp_el);
                    }
                    node.Opened = false;
                    //node.Opened = list_wp.Count > 0;
                    node.Children = list_wp;
                    list_location.Add(node);
                }
                root.Opened = list_location.Count > 0;
                root.Children = list_location;
                treeEquipment.Add(root);
                return treeEquipment;
            });
        }

        [HttpGet()]
        public async Task<ActionResult<TreeElementDto>> GetLeaf(int id, int type_leaf) 
        {
            return await Task<ActionResult>.Run(() => 
            {
                switch (type_leaf)
                {
                    case 1: 
                    case 2: 
                    case 3: 
                    case 4: 
                    break;
                    default:
                        break;
                }
                return Ok();
            });
        }

        //// GET: api/GetTreeDtoes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<GetTreeDto>> GetGetTreeDto(int id)
        //{
        //    var getTreeDto = await _context.GetTreeDto.FindAsync(id);

        //    if (getTreeDto == null)
        //    {
        //        return NotFound();
        //    }

        //    return getTreeDto;
        //}

        //// PUT: api/GetTreeDtoes/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGetTreeDto(int id, GetTreeDto getTreeDto)
        //{
        //    if (id != getTreeDto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(getTreeDto).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GetTreeDtoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/GetTreeDtoes
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<GetTreeDto>> PostGetTreeDto(GetTreeDto getTreeDto)
        //{
        //    _context.GetTreeDto.Add(getTreeDto);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetGetTreeDto", new { id = getTreeDto.Id }, getTreeDto);
        //}

        //// DELETE: api/GetTreeDtoes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGetTreeDto(int id)
        //{
        //    var getTreeDto = await _context.GetTreeDto.FindAsync(id);
        //    if (getTreeDto == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.GetTreeDto.Remove(getTreeDto);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool GetTreeDtoExists(int id)
        {
            return _context.GetTreeDto.Any(e => e.Id == id);
        }
    }
}
