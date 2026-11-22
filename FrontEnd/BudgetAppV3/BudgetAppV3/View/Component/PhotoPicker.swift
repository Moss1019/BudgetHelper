import SwiftUI
import PhotosUI

struct PhotoPicker: UIViewControllerRepresentable {
    @Binding var image: UIImage?
    @Binding var imageData: Data?

    func makeUIViewController(context: Context) -> PHPickerViewController {
        var config = PHPickerConfiguration()
        config.filter = .images
        let picker = PHPickerViewController(configuration: config)
        picker.delegate = context.coordinator
        return picker
    }

    func updateUIViewController(_ uiViewController: PHPickerViewController, context: Context) {}

    func makeCoordinator() -> Coordinator {
        Coordinator(self)
    }

    class Coordinator: NSObject, PHPickerViewControllerDelegate {
        let parent: PhotoPicker

        init(_ parent: PhotoPicker) {
            self.parent = parent
        }

        func picker(_ picker: PHPickerViewController, didFinishPicking results: [PHPickerResult]) {
            picker.dismiss(animated: true)
            guard let result = results.first else { return }

            // Load as UIImage
            result.itemProvider.loadObject(ofClass: UIImage.self) { object, error in
                DispatchQueue.main.async {
                    if let image = object as? UIImage {
                        self.parent.image = image
                        // Also capture as JPEG (80% quality)
                        self.parent.imageData = image.jpegData(compressionQuality: 0.8)
                    }
                }
            }
        }
    }
}
