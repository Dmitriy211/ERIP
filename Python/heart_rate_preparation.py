import argparse
from pathlib import Path
import pandas as pd
from datetime import datetime


def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('--path', type=str)
    parser.add_argument('--format', type=str, default="%d-%b-%y %H:%M:%S")
    return parser.parse_args()


if __name__ == '__main__':
    args = parse_args()
    heart_rate = pd.read_csv(args.path)
    heart_rate = heart_rate.drop_duplicates(subset=["At"])

    start = heart_rate['At'].iat[0]
    end = heart_rate['At'].iat[-1]
    start = datetime.strptime(start, args.format)
    end = datetime.strptime(end, args.format)
    duration = (end - start).seconds

    times = []
    for t in heart_rate['At']:
        timestamp = datetime.strptime(t, args.format) - start
        times.append(timestamp.seconds)

    heart_rates = list(heart_rate['Heartrate'])
    for i in range(0, duration):
        if i not in times:
            heart_rates.insert(i, heart_rates[i-1])

    data = {'Heartrate': heart_rates}
    heart_rate_fixed = pd.DataFrame(data)

    heart_rate_fixed.to_csv(str(Path(args.path).parent) + f'\\heartrate_processed.csv')
