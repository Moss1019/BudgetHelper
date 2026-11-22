import SwiftUI
import BudgetAppV3SharedV2

struct ReceiptView: View {
    @EnvironmentObject private var controller: ReceiptController
    
    @State private var isEditorPresented = false
    @State private var isUpdatingBudget = false
    
    var body: some View {
        NavigationView {
            ScrollView {
                LazyVStack(spacing: 24) {
                    if controller.isLoading {
                        ProgressView("Loading budget...")
                            .progressViewStyle(CircularProgressViewStyle())
                            .padding()
                    } else if let error = controller.errorMessage {
                        Text("Error: \(error)")
                            .font(.headline)
                            .foregroundColor(.red)
                            .padding()
                    } else {
                        buildContent()
                    }
                    
                    Spacer()
                }
                .padding(.horizontal, 16)
                .padding(.vertical, 8)
            }
            .navigationTitle("Budget Overview")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .navigationBarTrailing) {
                    Button(action: refresh) {
                        Image(systemName: "arrow.clockwise")
                    }
                    .disabled(controller.isLoading || isUpdatingBudget)
                    .opacity((controller.isLoading || isUpdatingBudget) ? 0.5 : 1.0)
                }
            }
            .navigationDestination(for: String.self) { receiptId in
                ReceiptLineView(receiptId: receiptId)
            }
            .sheet(isPresented: $isEditorPresented) {
                if let budget = controller.budget {
                    BudgetAmountEditor(monthlyLimit: budget.monthlyLimit) { value in
                        Task {
                            await controller.updateBudget(monthlyLimit: value)
                        }
                    }
                } else {
                    Text("")
                }
            }
        }
        .onAppear() {
            Task {
                await controller.reload()
            }
        }
    }
    
    private func refresh() {
        Task {
            if controller.isLoading || isUpdatingBudget {
                return
            }
            await controller.reload()
        }
    }
    
    private func buildContent() -> some View {
        VStack(spacing: 24) {
            if let budget = controller.budget {
                BudgetCard(budget: budget)
                    .onLongPressGesture {
                        isEditorPresented = true
                    }
                    .onTapGesture {
                        isEditorPresented = true
                    }
            }
            
            if let receipts = controller.receipts, !receipts.isEmpty {
                Text("Receipts")
                    .font(.headline)
                    .foregroundColor(.primary)
                
                ForEach(receipts, id: \.id) { receipt in
                    NavigationLink(destination: ReceiptLineView(receiptId: receipt.id)) {
                        ReceiptCard(receipt: receipt)
                    }
                }
            } else {
                Text("No receipts yet")
                    .font(.subheadline)
                    .foregroundColor(.secondary)
            }
        }
    }
}
