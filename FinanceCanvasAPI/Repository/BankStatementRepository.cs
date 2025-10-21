using FinanceCanvasAPI.Context.Entities;
using LiteDB;

namespace FinanceCanvasAPI.Repository;

public class BankStatementRepository(LiteDatabase db)
{
    private readonly ILiteCollection<BankStatement> _collection = db.GetCollection<BankStatement>("BankStatements");

    public void Insert(BankStatement bankStatement) => 
        _collection.Insert(bankStatement);

    public ILiteCollection<BankStatement> GetBankStatements() => 
        db.GetCollection<BankStatement>("bankstatements");
}