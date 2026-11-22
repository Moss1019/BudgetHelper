import SwiftUI

struct ReceiptDetectorView: View {
    @EnvironmentObject private var controller: ReceiptDetectorController
    
    var body: some View {
        VStack(spacing: 20) {
            Text("Receipt Detector")
                .font(.largeTitle)
                .padding(.top)
            
            if let image = controller.selectedImage {
                Image(uiImage: image)
                    .resizable()
                    .scaledToFit()
                    .frame(maxHeight: 300)
                    .cornerRadius(10)
            } else {
                Text("No image selected")
                    .foregroundColor(.secondary)
                    .padding()
            }
            
            if let error = controller.errorMessage {
                Text("Error: \(error)")
                    .foregroundColor(.red)
                    .padding()
            }
            
            if controller.isProcessing {
                ProgressView("Processing...")
                    .padding()
            }
            
            if let image = controller.selectedImage {
                VStack(spacing: 12) {
                    Button("Extract lines", action: { controller.processImageData() })
                    
                    Button("Clear image", action: { controller.clear() })
                }
                    
                
            } else {
                Button("Choose photo", action: { controller.isImagePickerPresented = true })
            }
            
            Spacer()
        }
        .sheet(isPresented: $controller.isImagePickerPresented) {
            PhotoPicker(image: $controller.selectedImage, imageData: $controller.selectedImageData)
        }
        .padding(.bottom)
    }
}

