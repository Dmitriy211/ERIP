import argparse
from pathlib import Path
import pandas as pd
import torch
from torch.utils.data import DataLoader
from tqdm import tqdm
from emonet.models import EmoNet
from erip_dataset import EripDataset


def load_emonet(device='cuda:0', n_expression=8):
    device = 'cuda:0'
    n_expression = 8
    state_dict_path = Path(__file__).parent.joinpath('emonet/pretrained', f'emonet_{n_expression}.pth')
    print(f'Loading the model from {state_dict_path}.')
    state_dict = torch.load(str(state_dict_path), map_location='cpu')
    state_dict = {k.replace('module.', ''): v for k, v in state_dict.items()}
    net = EmoNet(n_expression=n_expression).to(device)
    net.load_state_dict(state_dict, strict=False)
    return net


def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('--path', type=str)
    parser.add_argument('--device', type=str, default='cuda:0')
    parser.add_argument('--batch_size', type=int, default=32)
    parser.add_argument('--num_workers', type=int, default=12)
    return parser.parse_args()


if __name__ == '__main__':
    args = parse_args()

    net = load_emonet(device=args.device, n_expression=8)
    net.eval()

    data = EripDataset(args.path)
    data_loader = DataLoader(data, batch_size=args.batch_size, shuffle=False, num_workers=args.num_workers)

    data_frame = pd.DataFrame({
        'valence': [],
        'arousal': []
    })

    for index, images in enumerate(tqdm(data_loader)):
        images = images.to(args.device)
        with torch.no_grad():
            out = net(images)
            valence = out['valence'].cpu().numpy()
            arousal = out['arousal'].cpu().numpy()
            data = {
                'valence': valence,
                'arousal': arousal
            }
            sub_frame = pd.DataFrame(data)
            data_frame = data_frame.append(sub_frame, ignore_index=True)

    data_frame.to_csv(args.path + f'\\result.csv')




