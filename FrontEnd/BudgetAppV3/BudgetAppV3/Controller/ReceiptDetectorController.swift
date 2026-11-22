//
import SwiftUI
import Combine
import BudgetAppV3SharedV2

class ReceiptDetectorController: ObservableObject {
    private let service: ReceiptDetectorService
    
    @Published var selectedImage: UIImage?
    @Published var isImagePickerPresented = false
    @Published var isProcessing = false
    @Published var isLoading = true
    @Published var errorMessage: String?
    
    public init(service: ReceiptDetectorService) {
        self.service = service
    }
    
    func processImageData(_ data: Data) async {
        isProcessing = true
        errorMessage = nil
        
        guard let uiImage = UIImage(data: data) else {
            errorMessage = "Failed to decode image"
            isProcessing = false
            return
        }
        
        selectedImage = uiImage
        
        let base64String = data.base64EncodedString()
        let req = ReceiptExtractionRequest(base64Image: base64String)
        
        let extractRes = await service.extract(req)
        if !extractRes.isSuccessful {
            errorMessage = extractRes.message
        }
        
        isProcessing = false
    }
}
