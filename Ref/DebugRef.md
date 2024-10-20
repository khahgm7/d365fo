# Inventory\Costing
## Calculating cost of items
- BOMCalcCost::calcCostModel - Entrance point.

# Finance
## Posting
### Journal\Balance
- LedgerVoucherObject::addTrans - Add each journal trans to ledger voucher object to check.
-- LedgerVoucherObject::updateBalances - Update total balances of all lines for later check.

- LedgerVoucherObject::checkBalance - Check the balances.
-- LedgerVoucherObject::checkBalanceRound - Check the balances + Ledger rounding penny differences.