
select * from Carts;

--get all items in cart by user id
	
	create or alter procedure SP_GetAllCarts
	@userid int
	as
	begin
		begin try
			begin transaction;

			select 
				c.cartid, 
				c.bookid, 
				b.bookname, 
				b.author, 
				c.quantity, 
				c.unitprice, 
				b.bookimage, 
				c.ispurchased
			from carts c
			inner join books b 
				on c.bookid = b.bookid
			where c.userid = @userid and c.ispurchased = 0;

			commit transaction;
		end try
		begin catch
			rollback transaction;
			print 'error: ' + error_message();
		end catch
	end


-- to execute
	exec SP_GetAllCarts @UserId=1;

-----------------------------------------------------------------------------------------------------------

-- add book to cart

		create or alter procedure SP_AddBookToCart
			@userid int,
			@bookid int,
			@quantity int
		as
		begin
			begin try
				begin transaction;

				declare @discountprice int;
				select @discountprice = discountprice from books where bookid = @bookid;

				if exists (select 1 from carts where userid = @userid and bookid = @bookid)
				begin
					update carts 
					set quantity = @quantity 
					where userid = @userid and bookid = @bookid;

					print 'Updated cart Item !';
				end
				else
				begin
					insert into carts (userid, bookid, quantity, unitprice, addedat)
					values (@userid, @bookid, @quantity, @discountprice, getdate());

					print 'New cart item added !';
				end

				commit transaction;
			end try
			begin catch
				rollback transaction;
				print 'error: ' + error_message() +' !!!!!!!!';
			end catch
		end


-- to execute
	exec SP_AddBookToCart 
	@UserId=1,
    @BookId = 21,
    @Quantity = 1;

------------------------------------------------------------------------------------------------------------

-- remove book from cart
create or alter procedure SP_RemoveCartItem
    @userid int,
    @bookid int
as
begin
    begin try
        begin transaction;

        delete from carts 
        where userid = @userid and bookid = @bookid;

        print 'Removed book from cart!';

        commit transaction;
    end try
    begin catch
        rollback transaction;
        print 'Error: ' + error_message() + ' !!!!!!';
    end catch
end



-- execute
	exec SP_RemoveCartItem  @UserId = 1, @BookId = 21


-------------------------------------------------------------------------------------------------------------
-- update quantity of book in cart
create or alter procedure SP_UpdateCartQuantity
    @userid int,
    @bookid int,
    @quantity int
as
begin
    begin try
        begin transaction;

        update carts
        set quantity = @quantity
        where userid = @userid and bookid = @bookid;

        print 'Updated books quantity!';

        commit transaction;
    end try
    begin catch
        rollback transaction;
        print 'Error: ' + error_message() + ' !!!!!!!';
    end catch
end


-- execute
exec SP_UpdateCartQuantity @UserId = 1,	@BookId = 20, @Quantity = 5;
