import SwiftUI
import PhotosUI

struct ReceiptDetectorView: View {
    @EnvironmentObject private var controller: ReceiptDetectorController
    
//    @Binding var selectedImageData: Data?
    
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
            
            HStack(spacing: 16) {
                Button(action: { controller.isImagePickerPresented = true }) {
                    HStack {
                        Image(systemName: "camera")
                        Text("Take Photo")
                    }
                    .font(.headline)
                    .frame(maxWidth: .infinity)
                    .frame(height: 50)
                    .background(Color.blue)
                    .foregroundColor(.white)
                    .cornerRadius(10)
                }
                
//                PhotosPicker(selection: $selectedImageData, matching: .images, photoLibrary: .shared()) {
//                    HStack {
//                        Image(systemName: "photo.on.rectangle.angled")
//                        Text("Select Photo")
//                    }
//                    .font(.headline)
//                    .frame(maxWidth: .infinity)
//                    .frame(height: 50)
//                    .background(Color.green)
//                    .foregroundColor(.white)
//                    .cornerRadius(10)
//                }
            }
            .padding(.horizontal)
            
            Spacer()
        }
        .padding(.bottom)
    }
}

