﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DealerAPI.Data;

using DealerAPI.Models.DTO;

namespace DealerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcurementController : ControllerBase
    {
        private readonly ILogger<ProcurementController> _logger;
        private readonly ApplicationDbContext _db;
        public ProcurementController(ILogger<ProcurementController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProcurementFilterDto>>> GetFilter()
        {
            try
            {
                _logger.LogInformation("Getting filters");

                var filter = await _db.ProcurementFilters.ToListAsync();
                return Ok(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching sports: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("Procurement")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProcurementDto>>> FilterProcurement(int? Id)
        {
            try
            {
                // Check if breedId is null (no filter selected)
                if (!Id.HasValue)
                {
                    _logger.LogInformation("Getting Procurement");
                    // Retrieve all procurements if no filter is selected
                    var allProcurements = await _db.procDetails

                        .Include(p => p.Payment)

                        .Select(p => new ProcurementDto
                        {
                            PurchaseId = p.CarId,
                            CarName = p.CarName,
                            Variant = p.Variant,
                            Amount_due = p.Due_Amount,
                            Amount_paid = p.Paid_Amount,
                            Facility_Availed = p.Facility_Availed,
                            Invoice_Charges = p.Invoice_Charges,
                            Processing_charges = p.ProcessingCharges,

                            // Add the specific procurement field you want to include
                        })
                        .ToListAsync();

                    return Ok(allProcurements);
                }

                // Retrieve procurements filtered by breed ID
                var ProcurementFiltered = await _db.procDetails

                                            .Include(p => p.Payment)

                    .Where(P => P.FilterId == Id)
                    .Select(p => new ProcurementDto
                    {
                        PurchaseId = p.CarId,
                        CarName = p.CarName,
                        Variant = p.Variant,
                        Amount_due = p.Due_Amount,
                        Amount_paid = p.Paid_Amount,
                        Facility_Availed = p.Facility_Availed,
                        Invoice_Charges = p.Invoice_Charges,
                        Processing_charges = p.ProcessingCharges,



                        // Add the specific procurement field you want to include
                    })
                    .ToListAsync();

                if (ProcurementFiltered == null || !ProcurementFiltered.Any())
                {
                    return NotFound(); // Return 404 if no procurements found for the given ID
                }

                return Ok(ProcurementFiltered);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions appropriately
                // For simplicity, returning a 500 Internal Server Error here
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet("ProcurementStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProcurementStatusDto>>> GetProcurementStatus()
        {
            try
            {
                _logger.LogInformation("Getting Procurement Status");
                var procurementStatus = await _db.procDetails
                    .Include(c => c.Payment)
                        .Where(c => c.Status != null)
                    .Select(c => new ProcurementStatusDto
                    {
                        CarName = c.CarName,
                        Variant = c.Variant,
                        PurchaseId = c.CarId,
                        Status = c.Status
                    })
                    .ToListAsync();

                return Ok(procurementStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching cars with status: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("ProcurementClosed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProcurementColsedDto>>> ProcurementClosed()
        {
            try
            {
                _logger.LogInformation("Getting Procurement Closed");
                var procurementclosed = await _db.procDetails
                    .Include(c => c.Payment)
                        .Where(c => c.ClosedOn != null)

                    .Select(c => new ProcurementColsedDto
                    {
                        CarName = c.CarName,
                        Variant = c.Variant,
                        Amount_paid = c.Paid_Amount,
                        ColsedOn = c.ClosedOn


                    })
                    .ToListAsync();

                return Ok(procurementclosed);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching cars with status: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
