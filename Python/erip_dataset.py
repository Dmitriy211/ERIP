from torch.utils.data import Dataset
import torch
from pathlib import Path
from skimage import io
from torchvision import transforms


class EripDataset(Dataset):
    def __init__(self, root_path, image_format='*.png'):
        self.root_path = root_path
        self.format = image_format
        paths = Path(root_path).glob(image_format)
        self.keys = [file.resolve() for file in paths]
        print(self.keys[0])
        print(self.keys[-1])

    def __len__(self):
        return len(self.keys)

    def __getitem__(self, index):
        key = self.keys[index]
        image = io.imread(key)
        image = self.reformat_image(image)
        return image

    @staticmethod
    def reformat_image(image):
        transform_image = transforms.Compose([transforms.ToTensor()])
        image = transform_image(image).to(torch.float32)
        return image
