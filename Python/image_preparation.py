import numpy as np
import os
from pathlib import Path
import torch
import argparse
from skimage import io, img_as_ubyte
from skimage.transform import resize
from torchvision import transforms
import face_recognition
from tqdm import tqdm


def localize_face(image, padding=50):
    faces = face_recognition.face_locations(image, model="cnn")
    if len(faces) == 0:
        return None

    top, right, bottom, left = faces[0]
    top = max(top - padding, 0)
    bottom = min(bottom + padding, image.shape[0])
    left = max(left - padding, 0)
    right = min(right + padding, image.shape[1])

    image = image[top:bottom, left:right]
    return image


def reformat_image(image, device='cuda:0', size=(256, 256)):
    transform_image = transforms.Compose([transforms.ToTensor()])
    image = resize(image, size)
    image = transform_image(image).to(torch.float32)
    image = image[None, :].to(device)
    return image


def prepare_image(path, padding=50, size=(256, 256)):
    image = io.imread(path)
    image = localize_face(image, padding)

    if image is None:
        return None

    image = resize(image, size)
    return image

def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('--path', type=str)
    parser.add_argument('--size', type=tuple, default=(256, 256))
    parser.add_argument('--padding', type=int, default=200)
    parser.add_argument('--format', type=str, default='*.png')
    return parser.parse_args()


if __name__ == '__main__':
    args = parse_args()
    dir_path = args.path + '/processed/'
    isExist = os.path.exists(dir_path)
    if not isExist:
        os.makedirs(dir_path)

    image_paths = Path(args.path).glob(args.format)
    for image_path in tqdm(image_paths):
        image = prepare_image(image_path, padding=args.padding, size=args.size)
        if image is None:
            continue
        io.imsave(dir_path + image_path.name, img_as_ubyte(image))


