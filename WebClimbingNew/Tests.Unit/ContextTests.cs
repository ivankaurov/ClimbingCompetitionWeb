using System;
using System.Threading.Tasks;
using Climbing.Web.Database;
using Climbing.Web.Model.Logging;
using Climbing.Web.Tests.Unit.Utilities;
using Climbing.Web.Utilities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Climbing.Web.Tests.Unit
{
    public class ContextTests
    {
        [Theory]
        [AutoMoqData]
        public async Task ShouldSaveNewObject(ClimbingContext context, TestIdentityObject obj, Guid newId)
        {
            // Arrange
            obj.SetProperty(o => o.Id, newId);
            var ltr = new Ltr();
            ltr.AddNewObject(obj);

            await Task.Delay(1000);
            var testStart = DateTimeOffset.Now;

            // Act
            await context.LogicTransactions.AddAsync(ltr);
            await context.SaveChangesAsync();
            var testStop = DateTimeOffset.Now;

            // Arrange
            var actualLtrObj = await context.LtrObjects.SingleOrDefaultAsync(o => o.ObjectId == newId);
            Assert.NotNull(actualLtrObj);
            Assert.Equal(ChangeType.New, actualLtrObj.ChangeType);
            Assert.NotEmpty(actualLtrObj.Properties);
            Assert.NotNull(actualLtrObj.Ltr);
            Assert.Equal(actualLtrObj.LtrId, actualLtrObj.Ltr.Id);
            Assert.Equal(actualLtrObj.LtrId, ltr.Id);

            Assert.InRange(actualLtrObj.WhenCreated, testStart, testStop);
            Assert.InRange(actualLtrObj.WhenChanged, testStart, testStop);

            Assert.All(actualLtrObj.Properties, p => Assert.InRange(p.WhenCreated, testStart, testStop));
            Assert.All(actualLtrObj.Properties, p => Assert.InRange(p.WhenChanged, testStart, testStop));
        }
    }
}