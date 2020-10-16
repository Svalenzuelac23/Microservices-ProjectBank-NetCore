﻿using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Withdrawal.Model;

namespace MS.AFORO255.Withdrawal.Repository.Data
{
    public class ContextDatabase : DbContext, IContextDatabase
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) 
            : base(options) //ACA LE ESTMOS PASANDO DESDE EL CONTRUCTUR EL VALOR DE OPTIONES AL PARAMETRO DE LA CLASE BASE DBCONTEXT
        {
        }

        public DbSet<Transaction> Transaction { get; set; }
    }
}
