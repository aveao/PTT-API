import requests
import time

user_agent = "com.pttxd"
international_link = "https://pttws.ptt.gov.tr/cepptt/mssnvrPttaceaemaa/gonderitakipvepostakod/yurtDisiKargoSorgulaMSAEHPREMHMRGBAGDOGMAMA"
general_link = "https://pttws.ptt.gov.tr/cepptt/mssnvrPttaceaemaa/gonderitakipvepostakod/gonderisorgu2MSAEHPREMHMRGBAGDOGMAMA"
barcodes = {"BR123456789CD": "Left side is barcode code, right is name", "BR987654321CD": "you can put as many as you like"}

headers = {
    'User-Agent': user_agent,
}

while 1:
	for barcode in barcodes.keys():
		r = requests.post(international_link, data = {'barkod': barcode}, headers = headers)
		rj = r.json()
		events = rj["dongu"]
		if len(events) != 0:
			print("Info for {}:\n".format(barcodes[barcode]))
			for event in events:
				text = "{} on {}".format(event["event"], event["tarih"])
				if 'ofis' in event:
					text += " at {}".format(event["ofis"])
				print(text)
			print("---")
	time.sleep(60*5)
