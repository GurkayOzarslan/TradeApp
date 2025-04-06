import csv
from datetime import datetime
import os
import matplotlib.pyplot as plt

def log_model_report(
    model_name, window_size, feature_names,
    units_1, units_2, dropout_1, dropout_2, optimizer,
    param_count, rmse, mape,
    feature_importances=None,
    path="model_raporlari.csv"
):
    # Feature importance stringi hazırla
    if feature_importances:
        fi_str = "; ".join([f"{feat}:{round(imp, 6)}" for feat, imp in feature_importances])
    else:
        fi_str = ""

    try:
        with open(path, 'x', newline='') as f:
            writer = csv.writer(f)
            writer.writerow([
                "timestamp", "model_name", "window_size", "feature_names",
                "units_1", "units_2", "dropout_1", "dropout_2", "optimizer",
                "param_count", "RMSE", "MAPE", "feature_importances"
            ])
    except FileExistsError:
        pass

    with open(path, 'a', newline='') as f:
        writer = csv.writer(f)
        writer.writerow([
            datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
            model_name,
            window_size,
            ", ".join(feature_names),
            units_1,
            units_2,
            dropout_1,
            dropout_2,
            optimizer,
            param_count,
            float(rmse),
            float(mape),
            fi_str
        ])



def save_plot(title="plot", folder="plots"):

    os.makedirs(folder, exist_ok=True)  # klasör yoksa oluştur
    timestamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    filename = f"{folder}/{title}_{timestamp}.png"
    plt.savefig(filename, dpi=300, bbox_inches='tight')
    print(f"📸 Grafik kaydedildi: {filename}")
