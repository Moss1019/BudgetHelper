//
import SwiftUI
import Combine
import BudgetAppV3SharedV2

class ReceiptDetectorController: ObservableObject {
    private let service: ReceiptDetectorService
    
    @Published var selectedImage: UIImage?
    @Published var selectedImageData: Data?
    @Published var isImagePickerPresented = false
    @Published var isProcessing = false
    @Published var isLoading = true
    @Published var errorMessage: String?
    
    public init(service: ReceiptDetectorService) {
        self.service = service
    }
    
    func clear() {
        selectedImage = nil
        selectedImageData = nil
    }
    
    func processImageData() {
        isProcessing = true
        errorMessage = nil
        
        guard let data = selectedImageData else {
            errorMessage = "Failed to decode image"
            isProcessing = false
            return
        }
        
        let base64String = data.base64EncodedString()
        let req = ReceiptExtractionRequest(base64Image: base64String)
        
        Task {
            let extractRes = await service.extract(req)
            if !extractRes.isSuccessful {
                errorMessage = extractRes.message
            }
            
            isProcessing = false
        }
        
    }
}
