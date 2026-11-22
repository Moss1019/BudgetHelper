
import SwiftUI
import BudgetAppV3SharedV2

struct ReceiptLineView: View {
    @EnvironmentObject private var controller: ReceiptLineController
    
    private let receiptId: String
    
    init(receiptId: String) {
        self.receiptId = receiptId
    }
    
    var body: some View {
        NavigationView {
            ScrollView {
                LazyVStack(spacing: 24) {
                    if controller.isLoading {
                        ProgressView("Loading lines...")
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
            .navigationTitle("Receipt lines")
            .navigationBarTitleDisplayMode(.inline)
        }
        .onAppear() {
            Task {
                await controller.reload(receiptId)
            }
        }
    }
    
    private func buildContent() -> some View {
        VStack(spacing: 24) {
            if let lines = controller.lines, !lines.isEmpty {   
                ForEach(lines, id: \.id) { line in
                    ReceiptLineCard(line: line)
                }
            } else {
                Text("No lines yet")
                    .font(.subheadline)
                    .foregroundColor(.secondary)
            }
        }
    }
}
