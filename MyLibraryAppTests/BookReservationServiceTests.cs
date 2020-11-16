using FluentAssertions;
using Moq;
using MyLibraryApp.Entities;
using MyLibraryApp.Repository;
using MyLibraryApp.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace MyLibraryAppTests
{
    public class BookReservationServiceTests
    {
        [Fact]
        public void ReserveBook_if_book_available_reserve_and_set_reminder()
        {
            int memberId = 1;
            int bookId = 1;
            var reserveFrom = DateTimeOffset.Parse("01/01/2010");
            var reserveTo = DateTimeOffset.Parse("05/01/2010");

            var book = new Book
            {
                Id = bookId
            };

            var member = new Member
            {
                Id = memberId
            };

            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(r => r.Get(bookId))
                .Returns(book);

            var memberRepositoryMock = new Mock<IMemberRepository>();
            memberRepositoryMock.Setup(r => r.Get(memberId))
                .Returns(member);
            memberRepositoryMock.Setup(r => r.AddReservation(It.IsAny<Reservation>()));

            var bookReservationService = new BookReservationService(bookRepositoryMock.Object, memberRepositoryMock.Object);

            var reservation = bookReservationService.ReserveBook(memberId, bookId, reserveFrom, reserveTo);

            member.Reminders.Should().HaveCount(1);
            // reservation added:
            memberRepositoryMock.Verify(m => m.AddReservation(It.IsAny<Reservation>()), Times.Once());

            reservation.BookId.Should().Be(bookId);
            reservation.MemberId.Should().Be(memberId);
            reservation.To.Should().Be(reserveTo);
            reservation.From.Should().Be(reserveFrom);
        }

        [Fact]
        public void ReserveBook_if_book_not_available_cannot_reserve()
        {
            int memberId = 1;
            int bookId = 1;
            var reserveFrom = DateTimeOffset.Parse("02/01/2010");
            var reserveTo = DateTimeOffset.Parse("05/01/2010");

            var book = new Book
            {
                Id = bookId,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        From = DateTimeOffset.Parse("01/01/2010"),
                        To = DateTimeOffset.Parse("12/01/2010")
                    }
                }
            };

            var member = new Member
            {
                Id = memberId
            };

            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(r => r.Get(bookId))
                .Returns(book);

            var memberRepositoryMock = new Mock<IMemberRepository>();
            memberRepositoryMock.Setup(r => r.Get(memberId))
                .Returns(member);

            var bookReservationService = new BookReservationService(bookRepositoryMock.Object, memberRepositoryMock.Object);

            var reservation = bookReservationService.ReserveBook(memberId, bookId, reserveFrom, reserveTo);

            member.Reminders.Should().HaveCount(0);
            member.Reservations.Should().HaveCount(0);

            reservation.Should().BeNull();
        }

        [Fact]
        public void ReserveBook_if_user_has_3_reservations_cannot_reserve()
        {
            int memberId = 1;
            int bookId = 1;
            var reserveFrom = DateTimeOffset.Parse("02/01/2010");
            var reserveTo = DateTimeOffset.Parse("05/01/2010");

            var book = new Book
            {
                Id = bookId
            };

            var member = new Member
            {
                Id = memberId,
                Reservations = new List<Reservation>
                {
                    new Reservation{ From = DateTimeOffset.Parse("05/01/2040") },
                    new Reservation{ From = DateTimeOffset.Parse("05/01/2040") },
                    new Reservation{ From = DateTimeOffset.Parse("05/01/2040") }
                }
            };

            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(r => r.Get(bookId))
                .Returns(book);

            var memberRepositoryMock = new Mock<IMemberRepository>();
            memberRepositoryMock.Setup(r => r.Get(memberId))
                .Returns(member);

            var bookReservationService = new BookReservationService(bookRepositoryMock.Object, memberRepositoryMock.Object);

            var reservation = bookReservationService.ReserveBook(memberId, bookId, reserveFrom, reserveTo);

            member.Reminders.Should().HaveCount(0);
            member.Reservations.Should().HaveCount(3);

            reservation.Should().BeNull();
        }
    }
}
