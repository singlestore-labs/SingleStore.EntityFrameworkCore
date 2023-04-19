import json
import os
import sys

CLUSTER_ID_FILE = "CLUSTER_ID"
HOSTNAME_TMPL = "svc-{}-ddl.aws-frankfurt-1.svc.singlestore.com"


if __name__ == "__main__":

    home_dir = os.getenv("HOMEPATH")
    if home_dir is None:
        home_dir = os.getenv("HOME")

    with open(CLUSTER_ID_FILE, "r") as f:
        cluster_id = f.read().strip()

    hostname = HOSTNAME_TMPL.format(cluster_id)
    password = os.getenv("SQL_USER_PASSWORD")

    with open("./.circleci/config.json", "r") as f_in:
        config_content = f_in.read()

    config_content = config_content.replace("SINGLESTORE_HOST", hostname, 1)
    config_content = config_content.replace("SQL_USER_PASSWORD", password, 1)
    config_content = config_content.replace("SQL_USER_NAME", "admin", 1)

    if len(sys.argv) < 1:
        print("Not enough arguments to fill the config file!")
        exit(1)

    test_block = sys.argv[1]

    with open(f"test/{test_block}/config.json", "w") as f_out:
        f_out.write(config_content)

    with open(os.path.join(home_dir, "CONNECTION_STRING"), "w") as f_conn:
        f_conn.write(json.loads(config_content)["Data"]["ConnectionString"])
