using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIShopItem
    {
        private UIShop shop;
        public int shopItemId;
        public Text limitClass;
        private ItemDefine item;
        private ShopItemDefine shopItem { get; set; }
        public Text title;
        public Text count;
        public Text price;
        public Image icon;
        public Image background;
        public Sprite normalBg;
        public Sprite selectedBg;
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                this.background.overrideSprite = selected ? selectedBg : normalBg;
            }
        }
        void Start()
        {

        }

        public void SetShopItem(int id,ShopItemDefine shopItem,UIShop owner)
        {
            this.shop = owner;
            this.shopItemId = id;
            this.shopItem = shopItem;
            this.item = DataManager.Instance.Items[this.shopItem.ItemID];
            this.title.text = this.item.Name;
            this.count.text = shopItem.Count.ToString();
            this.price.text = shopItem.Price.ToString();
            this.limitClass.text = this.item.LimitClass.ToString();
            this.icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
        }

        public void OnSelect(BaseEventData eventData)
        {
            this.Selected = true;
            this.shop.SelectShopItem(this);
        }
    }
}
