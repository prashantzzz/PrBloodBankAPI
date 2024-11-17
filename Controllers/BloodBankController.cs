using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrBloodBankAPI.Models;

namespace PrBloodBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodBankController : ControllerBase
    {
        private static List<BloodBankEntry> _bloodBank = new List<BloodBankEntry>
{
    new BloodBankEntry
    {
        Id = 1,
        DonorName = "Prashant",
        Age = 28,
        BloodType = "A+",
        ContactInfo = "prashant2003@gmail.com",
        Quantity = 450,
        CollectionDate = DateTime.Now,
        ExpirationDate = DateTime.Now.AddDays(42),
        Status = "Available"
    },
    new BloodBankEntry
    {
        Id = 2,
        DonorName = "Sara Meena",
        Age = 35,
        BloodType = "O-",
        ContactInfo = "sara.m@email.com",
        Quantity = 400,
        CollectionDate = DateTime.Now.AddDays(-5),
        ExpirationDate = DateTime.Now.AddDays(37),
        Status = "Available"
    },
    new BloodBankEntry
    {
        Id = 3,
        DonorName = "Michael Chenna",
        Age = 42,
        BloodType = "B+",
        ContactInfo = "m.chenna@email.com",
        Quantity = 450,
        CollectionDate = DateTime.Now.AddDays(-10),
        ExpirationDate = DateTime.Now.AddDays(32),
        Status = "Requested"
    },
    new BloodBankEntry
    {
        Id = 4,
        DonorName = "Emita Kumari",
        Age = 29,
        BloodType = "AB+",
        ContactInfo = "emita.w@email.com",
        Quantity = 350,
        CollectionDate = DateTime.Now.AddDays(-15),
        ExpirationDate = DateTime.Now.AddDays(27),
        Status = "Available"
    },
    new BloodBankEntry
    {
        Id = 5,
        DonorName = "Robert Munder",
        Age = 45,
        BloodType = "O+",
        ContactInfo = "r.munder@email.com",
        Quantity = 500,
        CollectionDate = DateTime.Now.AddDays(-20),
        ExpirationDate = DateTime.Now.AddDays(22),
        Status = "Requested"
    },
    new BloodBankEntry
    {
        Id = 6,
        DonorName = "Viraj Thompson",
        Age = 31,
        BloodType = "A-",
        ContactInfo = "viraj.t@email.com",
        Quantity = 450,
        CollectionDate = DateTime.Now.AddDays(-30),
        ExpirationDate = DateTime.Now.AddDays(12),
        Status = "Available"
    },
    new BloodBankEntry
    {
        Id = 7,
        DonorName = "James Singh",
        Age = 38,
        BloodType = "B-",
        ContactInfo = "j.singh@email.com",
        Quantity = 400,
        CollectionDate = DateTime.Now.AddDays(-35),
        ExpirationDate = DateTime.Now.AddDays(7),
        Status = "Available"
    },
    new BloodBankEntry
    {
        Id = 8,
        DonorName = "Meera Garcia",
        Age = 27,
        BloodType = "AB-",
        ContactInfo = "m.garcia@email.com",
        Quantity = 350,
        CollectionDate = DateTime.Now.AddDays(-40),
        ExpirationDate = DateTime.Now.AddDays(2),
        Status = "Available"
    },
    new BloodBankEntry
    {
        Id = 9,
        DonorName = "Davi Khan",
        Age = 33,
        BloodType = "O+",
        ContactInfo = "d.khan@email.com",
        Quantity = 450,
        CollectionDate = DateTime.Now.AddDays(-41),
        ExpirationDate = DateTime.Now.AddDays(1),
        Status = "Requested"
    },
    new BloodBankEntry
    {
        Id = 10,
        DonorName = "Jennifer White",
        Age = 39,
        BloodType = "A+",
        ContactInfo = "j.white@email.com",
        Quantity = 400,
        CollectionDate = DateTime.Now.AddDays(-42),
        ExpirationDate = DateTime.Now,
        Status = "Expired"
    }
};

        // 1.create new entry
        [HttpPost]
        public ActionResult<BloodBankEntry> CreateEntry(BloodBankEntry entry)
        {
            // validate expiry date
            if (entry.ExpirationDate <= entry.CollectionDate)
            {
                return BadRequest("Expiration date must be after collection date");
            }

            // Auto generate ID
            entry.Id = _bloodBank.Any() ? _bloodBank.Max(e => e.Id) + 1 : 1;
            _bloodBank.Add(entry);

            return CreatedAtAction(nameof(GetEntryById), new { id = entry.Id }, entry);
        }


        // 2.get all entries with filter and sort
        [HttpGet]
        public ActionResult<IEnumerable<BloodBankEntry>> GetAllEntries(
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? bloodType = null,
            [FromQuery] string? status = null,
            [FromQuery] string? donorName = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? minAge = null,
            [FromQuery] int? maxAge = null)
        {
            var query = _bloodBank.AsQueryable();

            // filters
            query = ApplyFilters(query, bloodType, status, donorName, startDate, endDate, minAge, maxAge);

            // sorting
            query = ApplySorting(query, sortBy, sortOrder);

            var results = query.ToList();
            if (!results.Any())
            {
                return NotFound("No matching entries found");
            }

            return Ok(results);
        }

        private IQueryable<BloodBankEntry> ApplyFilters(
            IQueryable<BloodBankEntry> query,
            string? bloodType,
            string? status,
            string? donorName,
            DateTime? startDate,
            DateTime? endDate,
            int? minAge,
            int? maxAge)
        {
            // blood Type 
            if (!string.IsNullOrEmpty(bloodType))
            {
                query = query.Where(e => e.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase));
            }

            // filter by status 
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            // donor name 
            if (!string.IsNullOrEmpty(donorName))
            {
                query = query.Where(e => e.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase));
            }

            // date range
            if (startDate.HasValue)
            {
                query = query.Where(e => e.CollectionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.CollectionDate <= endDate.Value);
            }

            // age range 
            if (minAge.HasValue)
            {
                query = query.Where(e => e.Age >= minAge.Value);
            }

            if (maxAge.HasValue)
            {
                query = query.Where(e => e.Age <= maxAge.Value);
            }

            return query;
        }

        private IQueryable<BloodBankEntry> ApplySorting(
            IQueryable<BloodBankEntry> query,
            string? sortBy,
            string? sortOrder)
        {
            var isAscending = sortOrder?.ToLower() != "desc";

            switch (sortBy?.ToLower())
            {
                case "bloodtype":
                    query = isAscending ?
                        query.OrderBy(e => e.BloodType) :
                        query.OrderByDescending(e => e.BloodType);
                    break;

                case "collectiondate":
                    query = isAscending ?
                        query.OrderBy(e => e.CollectionDate) :
                        query.OrderByDescending(e => e.CollectionDate);
                    break;

                case "expirationdate":
                    query = isAscending ?
                        query.OrderBy(e => e.ExpirationDate) :
                        query.OrderByDescending(e => e.ExpirationDate);
                    break;

                case "status":
                    query = isAscending ?
                        query.OrderBy(e => e.Status) :
                        query.OrderByDescending(e => e.Status);
                    break;

                case "donorname":
                    query = isAscending ?
                        query.OrderBy(e => e.DonorName) :
                        query.OrderByDescending(e => e.DonorName);
                    break;

                case "age":
                    query = isAscending ?
                        query.OrderBy(e => e.Age) :
                        query.OrderByDescending(e => e.Age);
                    break;

                case "quantity":
                    query = isAscending ?
                        query.OrderBy(e => e.Quantity) :
                        query.OrderByDescending(e => e.Quantity);
                    break;

                default:
                    // Default sorting by Id
                    query = isAscending ?
                        query.OrderBy(e => e.Id) :
                        query.OrderByDescending(e => e.Id);
                    break;
            }

            return query;
        }


        // 3.get entry by ID
        [HttpGet("{id}")]
        public ActionResult<BloodBankEntry> GetEntryById(int id)
        {
            var entry = _bloodBank.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }


        // 4.update entry
        [HttpPut("{id}")]
        public IActionResult UpdateEntry(int id, BloodBankEntry entry)
        {
            var existingEntry = _bloodBank.FirstOrDefault(e => e.Id == id);
            if (existingEntry == null)
            {
                return NotFound();
            }

            // validating expiry date
            if (entry.ExpirationDate <= entry.CollectionDate)
            {
                return BadRequest("Expiration date must be after collection date");
            }

            existingEntry.DonorName = entry.DonorName;
            existingEntry.Age = entry.Age;
            existingEntry.BloodType = entry.BloodType;
            existingEntry.ContactInfo = entry.ContactInfo;
            existingEntry.Quantity = entry.Quantity;
            existingEntry.CollectionDate = entry.CollectionDate;
            existingEntry.ExpirationDate = entry.ExpirationDate;
            existingEntry.Status = entry.Status;

            return NoContent();
        }


        // 5.delete entry
        [HttpDelete("{id}")]
        public IActionResult DeleteEntry(int id)
        {
            var entry = _bloodBank.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }

            _bloodBank.Remove(entry);
            return NoContent();
        }


        // 6.pagination
        [HttpGet("page")]
        public ActionResult<IEnumerable<BloodBankEntry>> GetPaginatedEntries(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            var paginatedItems = _bloodBank
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return Ok(paginatedItems);
        }


        // 7.search by blood type
        [HttpGet("search")]
        public ActionResult<IEnumerable<BloodBankEntry>> SearchEntries(
            [FromQuery] string? bloodType = null,
            [FromQuery] string? status = null,
            [FromQuery] string? donorName = null)
        {
            var query = _bloodBank.AsQueryable();

            if (!string.IsNullOrEmpty(bloodType))
            {
                query = query.Where(e => e.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(donorName))
            {
                query = query.Where(e => e.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase));
            }

            var results = query.ToList();
            if (!results.Any())
            {
                return NotFound("No matching entries found");
            }

            return Ok(results);
        }
    }
}
