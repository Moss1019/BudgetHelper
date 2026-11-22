import Foundation

public class BudgetClient {
    let client = ApiClient()
    
    public init(){}
    
    public func get() async -> OpResult<Budget> {
        await client.get(Budget.self, remoteUrl: "api/budgets")
    }
    
    public func update(_ budget: Budget) async -> OpResult<Budget> {
        await client.post(Budget.self, remoteUrl: "api/budgets", value: budget)
     }
}
