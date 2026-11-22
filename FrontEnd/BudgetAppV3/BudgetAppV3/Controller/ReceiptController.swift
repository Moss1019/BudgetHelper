import Foundation
import SwiftUI
import Combine
import BudgetAppV3SharedV2

@MainActor
class ReceiptController: ObservableObject {
    private var budgetService: BudgetService
    private var receiptService: ReceiptService
    
    @Published var budget: Budget?
    @Published var receipts: [Receipt]?
    
    @Published var isLoading = true
    @Published var errorMessage: String?
    
    init(budgetService: BudgetService, receiptService: ReceiptService) {
        self.budgetService = budgetService
        self.receiptService = receiptService
    }
    
    func updateBudget(monthlyLimit: Decimal) async -> OpResult<Budget> {
        let updateRes = await budgetService.update(Budget(id: 0, monthlyLimit: monthlyLimit))
        if updateRes.isSuccessful {
            budget = updateRes.content!
        }
        return updateRes
    }
    
    func reload() async {
        isLoading = true
        errorMessage = nil
        
        // Load budget
        let budgetResult = await budgetService.get()
        
        // Check if task was cancelled during the network call
        guard !Task.isCancelled else {
            isLoading = false
            errorMessage = "Task was cancelled"
            return
        }
        
        if budgetResult.isSuccessful, let budget = budgetResult.content {
            self.budget = budget
        } else {
            errorMessage = budgetResult.message ?? "Failed to load budget"
            isLoading = false
            return
        }
        
        // Load receipts
        let receiptsResult = await receiptService.getAll()
        
        // Check if task was cancelled during the network call
        guard !Task.isCancelled else { return }
        
        if receiptsResult.isSuccessful {
            self.receipts = receiptsResult.content ?? []
        } else {
            // Don't fail entirely if receipts fail to load
            self.receipts = []
        }
        
        isLoading = false
    }
}
