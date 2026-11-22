import SwiftUI

public class BudgetService {
    let client = BudgetClient()
    
    public init() {}
    
    public func get() async -> OpResult<Budget> {
        await client.get()
    }
    
    public func update(_ budget: Budget) async -> OpResult<Budget> {
        await client.update(budget)
    }
}
