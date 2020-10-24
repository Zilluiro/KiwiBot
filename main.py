import telebot
import requests
import xml.etree.cElementTree as ET

bot = telebot.TeleBot('Токен сюда')

@bot.message_handler(commands=['start'])
def start_message(message):
    bot.send_message(message.chat.id, 'Привет, ты написал мне /start')

@bot.message_handler(commands=['last'])
def photo_message(message):
	response = requests.get('https://yande.re/post.json?limit=1')
	Json = response.json()
	print(Json[0]["file_url"])
	bot.send_photo(message.chat.id, str(Json[0]["file_url"]))    


bot.polling()
