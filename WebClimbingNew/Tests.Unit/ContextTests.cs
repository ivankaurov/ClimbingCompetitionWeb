using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Climbing.Web.Common.Service.Repository;
using Climbing.Web.Database;
using Climbing.Web.Model;
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
        public async Task ShouldSaveUpdateAndDeleteNewObject(IUnitOfWork context, string oldName, string code, string newName)
        {
            // Arrange (add)
            var testTeam = new Team
            { 
                Name = oldName,
                Code = code,
            };
            var addStart = DateTimeOffset.UtcNow;

            // Act (add)
            await context.Repository<Team>().AddAsync(testTeam);
            await context.SaveChangesAsync();
            var addStop = DateTimeOffset.UtcNow;
            var ltrObject = await context.Repository<LtrObject>().SingleAsync(o => o.ObjectId == testTeam.Id);

            // Assert (add)
            var actual = await context.Repository<Team>().SingleAsync(t => t.Id == testTeam.Id);
            var whenCreatedBak = actual.WhenCreated;
            Assert.Equal(testTeam.Name, actual.Name);
            Assert.InRange(actual.WhenChanged, addStart, addStop);
            Assert.InRange(actual.WhenCreated, addStart, addStop);
            Assert.Equal(ChangeType.New, ltrObject.ChangeType);
            this.AssertLtrObject(testTeam, ltrObject);

            // Arrange
            var changeStart = DateTimeOffset.UtcNow;

            // Act (change)
            actual.Name = newName;
            context.Repository<Team>().Update(actual);
            await context.SaveChangesAsync();
            var changeStop = DateTimeOffset.UtcNow;
            var changeLtr = await context.Repository<LtrObject>().SingleAsync(o => o.ObjectId == actual.Id && o.ChangeType == ChangeType.Update);
            var changedActual = await context.Repository<Team>().SingleAsync(t => t.Id == actual.Id);

            // Assert (change)
            Assert.Equal(newName, changedActual.Name);
            Assert.Equal(whenCreatedBak, changedActual.WhenCreated);
            Assert.InRange(changedActual.WhenChanged, changeStart, changeStop);
            this.AssertLtrObject(changedActual, changeLtr);

            // Act (delete)
            context.Repository<Team>().Remove(changedActual);
            await context.SaveChangesAsync();
            var removeLtr = await context.Repository<LtrObject>().SingleAsync(o => o.ObjectId == changedActual.Id && o.ChangeType == ChangeType.Delete);

            // Assert
            Assert.Null(await context.Repository<Team>().SingleOrDefaultAsync(t => t.Id == actual.Id));
            this.AssertLtrObject(changedActual, removeLtr);
        }

        private void AssertLtrObject(Team expected, LtrObject actual)
        {
            this.AssertLtrObjectProperty(expected, actual, t => t.Name);
            this.AssertLtrObjectProperty(expected, actual, t => t.Code);
        }

        private void AssertLtrObjectProperty<TObj, TProperty>(TObj expected, LtrObject actual, Expression<Func<TObj, TProperty>> propertyExpr)
        {
            var propertyName = ((MemberExpression)propertyExpr.Body).Member.Name;
            var expectedValue = propertyExpr.Compile().Invoke(expected);
            var actualPV = Assert.Single(actual.Properties.Where(p => p.PropertyName == propertyName));
            Assert.Equal(expectedValue?.ToString(), actualPV.Value);
        }
    }
}