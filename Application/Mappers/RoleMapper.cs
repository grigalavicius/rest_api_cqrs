using System;
using System.ComponentModel.DataAnnotations;
using Application.Extensions;
using DataStore.Models;

namespace Application.Mappers
{
    internal class RoleMapper
    {
        private const string exceptionMessage = "Invalid role type";

        public static string MapOut(Role role)
        {
            return role switch
            {
                Role.Ceo => Role.Ceo.GetAttribute<DisplayAttribute>().Name,
                Role.Administrator => Role.Administrator.GetAttribute<DisplayAttribute>().Name,
                Role.Seller => Role.Seller.GetAttribute<DisplayAttribute>().Name,
                Role.Waiter => Role.Waiter.GetAttribute<DisplayAttribute>().Name,
                _ => throw new ArgumentException(exceptionMessage)
            };
        }

        public static Role MapIn(int role)
        {
            return role switch
            {
                (int)Role.Ceo => Role.Ceo,
                (int)Role.Administrator => Role.Administrator,
                (int)Role.Seller => Role.Seller,
                (int)Role.Waiter => Role.Waiter,
                _ => throw new ArgumentException(exceptionMessage)
            };
        }
    }
}