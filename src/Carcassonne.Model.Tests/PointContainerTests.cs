using System;
using FluentAssertions;
using Xunit;

namespace Carcassonne.Model.Tests
{
    public class PointContainerTests
    {
        [Fact]
        public void Add_WithOneOneSidedRegion_ShouldHaveOneOpenEdges()
        {
            //arrange
            var container = new PointContainer(EdgeRegionType.City);

            //act
            container.Add(new CityEdgeRegion(EdgeDirection.South));

            //assert
            container.OpenEdges.Should().Be(1);
        }

        [Fact]
        public void Add_WithTwoOneSidedRegion_ShouldHaveNoOpenEdges()
        {
            //arrange
            var container = new PointContainer(EdgeRegionType.City);

            //act
            container.Add(new CityEdgeRegion(EdgeDirection.South));
            container.Add(new CityEdgeRegion(EdgeDirection.North));

            //assert
            container.OpenEdges.Should().Be(0);
        }

        [Fact]
        public void Add_WithOneTwoSidedRegion_ShouldHaveTwoOpenEdges()
        {
            //arrange
            var container = new PointContainer(EdgeRegionType.City);

            //act
            container.Add(new CityEdgeRegion(EdgeDirection.South, EdgeDirection.East));

            //assert
            container.OpenEdges.Should().Be(2);
        }

        [Fact]
        public void Add_WithTwoTwoSidedRegion_ShouldHaveTwoOpenEdges()
        {
            //arrange
            var container = new PointContainer(EdgeRegionType.City);

            //act
            container.Add(new CityEdgeRegion(EdgeDirection.South, EdgeDirection.East));
            container.Add(new CityEdgeRegion(EdgeDirection.West, EdgeDirection.South));

            //assert
            container.OpenEdges.Should().Be(2);
        }

        [Fact]
        public void Add_WithThreeTwoSidedRegion_ShouldHaveTwoOpenEdges()
        {
            //arrange
            var container = new PointContainer(EdgeRegionType.City);

            //act
            container.Add(new CityEdgeRegion(EdgeDirection.South, EdgeDirection.East));
            container.Add(new CityEdgeRegion(EdgeDirection.West, EdgeDirection.South));
            container.Add(new CityEdgeRegion(EdgeDirection.West, EdgeDirection.North));

            //assert
            container.OpenEdges.Should().Be(2);
        }

        [Fact]
        public void Merge_SingleContainerWithTwoOpenEdgesWithSelf_ShouldHaveNoOpenEdges()
        {
            //arrange
            var container = new PointContainer(EdgeRegionType.City);
            container.Add(new CityEdgeRegion(EdgeDirection.South, EdgeDirection.East));

            //act
            container.Merge(container);

            //assert
            container.OpenEdges.Should().Be(0);
        }

        private class TestPointContainer : PointContainer
        {
            public TestPointContainer(EdgeRegionType type) : base(type)
            {
            }

            public bool TestUpdateEdges(IEdgeRegion r)
            {
                return UpdateEdges(r);
            }
        }
    }
}