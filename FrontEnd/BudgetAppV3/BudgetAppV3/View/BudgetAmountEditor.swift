import SwiftUI
import BudgetAppV3SharedV2

struct BudgetAmountEditor: View {
    @Environment(\.dismiss) private var dismiss
    
    let onSave: (Decimal) -> Void

    @State private var tempAmount: String
    @State private var errorMessage: String?

    init(monthlyLimit: Decimal, onSave: @escaping (Decimal) -> Void) {
        self.onSave = onSave
        _tempAmount = State(
            initialValue: monthlyLimit.formatted(.number.precision(.fractionLength(2)))
        )
    }
    
    var body: some View {
        NavigationStack {
            VStack(spacing: 16) {
                if let error = errorMessage {
                    Text(error)
                        .foregroundColor(.red)
                        .font(.caption)
                }
                
                TextField("Enter amount", text: $tempAmount)
                    .keyboardType(.decimalPad)
                    .textInputAutocapitalization(.never)
                    .autocorrectionDisabled()
                    .textFieldStyle(RoundedBorderTextFieldStyle())
                    .padding(.horizontal)
                
                HStack {
                    Button("Cancel") {
                        dismiss()
                    }
                    .font(.body)
                    .padding(.horizontal)
                    
                    Spacer()
                    
                    Button("Save") {
                        saveAmount()
                    }
                    .disabled(tempAmount.isEmpty)
                    .font(.body)
                    .foregroundColor(.accentColor)
                    .padding(.horizontal)
                }
                .padding(.bottom, 8)
            }
            .navigationTitle("Edit Budget")
            .navigationBarTitleDisplayMode(.inline)
        }
    }
    
    private func saveAmount() {
        guard let value = Decimal.init(string: tempAmount) else {
            errorMessage = "Please enter a valid amount"
            return
        }
        
        onSave(value)
        dismiss()
    }
    
    private func formatCurrency(_ amount: Decimal) -> String {
        let formatter = NumberFormatter()
        formatter.numberStyle = .currency
        formatter.locale = Locale.current
        return formatter.string(from: NSDecimalNumber(decimal: amount)) ?? ""
    }
}
